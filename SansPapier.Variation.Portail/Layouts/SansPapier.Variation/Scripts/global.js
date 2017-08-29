/// <reference name="MicrosoftAjax.js"/>
/// <reference path="/Scripts/jquery-1.5.1-vsdoc.js" />
/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\SP.debug.js" />


var _original_onclick;
var _urls = { origine: null, destination: null };
var global = this;
var ListDocPDF = {};


/* OVERRIDE Core.JS */
(function () {
	var overrideMenu = function () {
		if (SP && SP.UI && SP.UI.AspMenu) {
			SP.UI.AspMenu.prototype.showSubMenu = function () { return false; }
		} else {
			setTimeout(overrideMenu, 100);
		}
	}
	overrideMenu();
})();
/* FIN OVERRIDE */

$(document).ready(function () {

    var _formSeance = $("<div style='display:none;' id='boiteProchaineSeancePopup'>\
    <div class='inner'>\
    <div class='fichier'>\
        <div class='destination' />\
        <div class='origine' />\
    </div>\
    <ul>\
        <li><a href='#' class='button ico pdf lecture'>"+_libelleOuvrir+"</a></li>\
        <li><a href='#' class='button ico inexistant annoter'>"+_libelleOuvrirAnnoter+"</a></li>\
        <li><a href='#' class='button ico anterieure inactif'>"+_libelleVersionsAnterieures+"</a></li>\
    </ul>\
    </div>\
    </div>");

    //var menu = $("#zz13_RootAspMenu").html();
    //$("#zz13_RootAspMenu").html("<li class='Static'></li>" + menu);

    // Titre du comité
	$(".headerPiv span span").html($("#rootDescription tr:first").attr("title"));

	// verifiction si appareil est un Iphone,Ipod ou Ipad)
	var deviceAgent = navigator.userAgent.toLowerCase();
	var agentID = deviceAgent.match(/(iphone|ipod|ipad)/);

	if (agentID) {

		// ajout d'un classe CSS sur le div "s4-workspace" pour retirer la barre déroulante
		// $("#s4-workspace").addClass("correctionIpad");
		$("body").attr("scroll", "yes").addClass("correctionIpad");
		$(".btnTailleTexte").attr("style", "dipslay:none;");
		$(".btnImprimer").attr("style", "dipslay:none;");


	}

	// Arrondi les coin des elements ayant la classe 'rounded-corners'
	$(".rounded-corners").corner("7px");

	// Call stylesheet init so that all stylesheet changing functions
	// will work.
	$.stylesheetInit();

	// This code loops through the stylesheets when you click the link with
	// an ID of "toggler" below.
	$('#toggler').bind('click',
		 function (e) {
		 	$.stylesheetToggle();
		 	return false;
		 }
	);

	// When one of the styleswitch links is clicked then switch the stylesheet to
	// the one matching the value of that links rel attribute.
	$('.styleswitch').bind('click',
	 function (e) {
	 	$.stylesheetSwitch(this.getAttribute('rel'));
	 	return false;
	 	SP
	 }
	 );


	// Champs recherche
	$(".btnRechercher").click(function (e) {
		//        var elemRecherche = $("#champsRecherche").val();
		//        var monUrl = $("#urlRecherche").val();
		//        window.location = monUrl + "?k=" + elemRecherche;
		window.location = $("#urlRecherche").val() + "?k=" + $("#champsRecherche").val();
	})
	$("#champsRecherche").bind("keypress", function (e) {

		if (e.keyCode && e.keyCode == 13) {
			e.preventDefault();
			$(".btnRechercher").click();
		}
	});

	// Navigation 
	// -----------------------------------------------------------------------------------------------------
	$(".menu-horizontal ul > li > a:not(:last)").each(function () {
		if ($(this).next("ul").length > 0) {
			$(this).menu({
				target: $(this).next("ul"),
				'offset-x': {
					alignTo: "left",
					offset: 0,
					start: "parent"
				},
				'offset-y': {
					alignTo: "bottom",
					offset: -3,
					start: "parent"
				}
			});
		}
	});
	// On aligne le dernier menu pour ne pas qu'il sorte de l'interface
	$(".menuGlobal > li > a:last").each(function () {
		if ($(this).next("ul").length > 0) {
			var width = -$(this).next("ul").outerWidth();
			$(this).menu({
				target: $(this).next("ul"),
				'offset-x': {
					alignTo: "right",
					offset: width,
					start: "parent"
				},
				'offset-y': {
					alignTo: "bottom",
					offset: -3,
					start: "parent"
				}
			});
		}
	});
	// Ribbon
	// --------------------------------------------------------------------
	$("span.ms-cui-row-tworow:empty").closest("li").width("150px");
	$("span.ms-cui-row-tworow:empty").remove();

	// Bouton Imprimer
	$(".btnImprimer").click(function (e) { e.preventDefault(); window.print(); });

	// Conversion des dates de nouvelles
	// -----------------------------------------------------------------------------------------------------
	var mois = ["janvier", "février", "mars", "avril", "mai", "juin", "juillet", "août", "septembre", "octobre", "novembre", "décembre"];
	$(".dateNouvelle").each(function () {
		var dateTime = $(this).text().split(" ");
		var date = dateTime[0].split("-");

		if (date && date.length > 0) {
			try {
				var moisText = (date[1].charAt(0) == "0") ? mois[parseInt(date[1].substr(1)) - 1] : mois[parseInt(date[1]) - 1];
				var string = date[2] + " " + moisText + " " + date[0];
				if (dateTime.length > 1) {
					var time = dateTime[1].split(":");
					if (dateTime[1] != "00:00:00") {
						string += ", à " + time[0] + "h" + time[1];
					}
				}
				$(this).html(string);
			} catch (er) { }
		}
	});
	// Populate liste email
	// -----------------------------------------------------------------------------------------------------
	var $target = $('#listeCourrielMembre');
	if ($target.find("a.button").length == 0) {
	    $target.find(".listeCourrielMembreInner").append($("<div class='chargement'>" + _libelleChargementCourriel + "</div>"));
		$target.find(".listeCourrielMembreInner").append($("<a class='button btnRedigerCourriel btn btnGris ' href='#'>" + _libelleRedigerCourriel + "</a>"));
	}
	// On récupère les événements du bouton
	var events = $target.find("a.button").data('events');
	// Si le click n'est pas déjà bindé, on bind la fonction pour régider un courriel
	if (!(events && events.click)) {
		// Bouton rédiger le courriel
		// concatène les emails des personnes sélectionnées pour en faire une string mailto
		// TODO : Changer les avertissements
		$target.find("a.button").bind("click.rediger", function () {
			var mailtoString = "";
			var mailtoCCString = "";
			// Mailto
			$(this).parent().find("li.selected:not('.group,.cc') .courriel").each(function () {

				if (navigator.platform.indexOf("iPad") != -1) {
					mailtoString += "," + $(this).html().replace(/;/g, ",");
				}
				else {
					mailtoString += ";" + $(this).html();
				}


			});
			// CC 
			$(this).parent().find("li.selected.cc .courriel").each(function () {

				if (navigator.platform.indexOf("iPad") != -1) {
					mailtoCCString += "," + $(this).html();
				}
				else {
					mailtoCCString += ";" + $(this).html();
				}
			});
			if (mailtoString.length > 0) {
				mailtoString += "?cc=" + mailtoCCString.substr(1);
			}
			if (mailtoString.length == 0) {
			    alert(_messageSelectionAdresse);
				return false;
			}
			//if (mailtoString.length > 1024) {
			//	alert("Vous avez dépassez la limite de caractères permise, nous ne garantissons pas le fonctionnement.");
			//}
			$(this).attr("href", "mailto:" + mailtoString.substr(1));
			$('.btnEnvoiCourriel').menu('close');
		});
	}
	// Création de la liste à partir d'un fichier JSON retourné par un service web
	if ($target.find("ul").length == 0) {
		$.ajax({
		    url: _RootVariation + "_vti_bin/ListData.svc/ListeCourriels",
			dataType: "json",
			success: function (data) {
				$target.find(".chargement").remove();
				// Création de la liste de courriels
				data = data.d.results;

				var groupQueue = [];
				var $list = $("<ul />");

				for (var i = 0; i < data.length; i++) {
					// Pour éviter qu'il y ait des "null" à l'affichage
					if (!data[i].Prenom) {
						data[i].Prenom = "";
					}
					if (!data[i].Nom) {
						data[i].Nom = "";
					}
					if (!data[i].Role2) {
					    data[i].Role2 = "";
					}
					else {
					    data[i].Role2 = ", " + data[i].Role2;
					}

					// ----------------------------------------------------
					// SI c'est un groupe, on l'ajoute dans la Queue pour les ajouter au visuel plus tard
					if (data[i].EstUnGroupe) {
					    groupQueue.push($("<li class='group'><div class='check'></div><div class='lblPrenom'>" + data[i].Prenom + " " + data[i].Nom + "</div><div>" + data[i].Role2 + "</div><div class='courriel'>" + data[i].Courriel + "</div></li>"));
					} else if (data[i].EstEnCC) {
						$list.append($("<li class='cc'><div class='lblPrenom'>" + data[i].Prenom + " " + data[i].Nom + "</div><div>" + data[i].Role2 + "</div><div class='courriel'>" + data[i].Courriel + "</div></li>"));
					} else {
						$list.append($("<li><div class='check'></div><div class='lblPrenom'>" + data[i].Prenom + " " + data[i].Nom + "</div><div>" + data[i].Role2 + "</div><div class='courriel'>" + data[i].Courriel + "</div></li>"));
					}
				}
				$list.append("<li class='espaceur'></li>");
				// On boucle sur la Queue de groupes pour les ajouter au visuel
				for (var i = 0; i < groupQueue.length; i++) {
					$list.append(groupQueue[i]);
				}
				// On bind le click des éléments li
				$list.find("li").not(".espaceur").bind("click", function () {
					// SI c'est un groupe, on ajoute la classe "selected" à tous les li qui contiennent une adresse 
					// email contenue dans le groupe
					if (this.className.match("group")) {
						if (!this.className.match("selected")) {
							var emails = $(this).find(".courriel").html().split(";");

							$(this).parent().find("li:not('.group') .courriel").each(function () {
								for (var i = 0; i < emails.length; i++) {
									if ($(this).html().match(emails[i])) {
										$(this).parent().addClass("selected");
									}
								}
							});
						}
						// SI ce n'est pas un groupe, on toggle la classe "selected"
					} else {
						$(this).toggleClass("selected");
					}
				});
				$target.find(".listeCourrielMembreInner").prepend($list);
			}
		});
	}

	// Menu envoi de courriel
	// ----------------------------------------------------------------------------------------------------------------
	var btnSelector = '.btnEnvoiCourriel';
	$(btnSelector).menu({
		target: $('#listeCourrielMembre'),
		'width': "auto",
        'position':"relative",
		'offset-y': { alignTo: 'bottom', offset: -32, start: "parent" },
		'offset-x': { alignTo: 'left', offset: -8, start: "parent" },
		
		beforeOpen: function () {
			this.target.parent().css("z-index", "0");
			this.target.find(".selected").removeClass("selected");
			this.target.find(".btnEnvoiCourrielOV").bind('click', function () {
				$(btnSelector).menu('close');
			});
		},
		close: function () {
			this.target.parent().css("z-index", "0");
		}

	});

	// Ajouter un lien utile
	// ------------------------------------------------------------------
	// FORM pour la saisie des informations
	var $formAjouterLien = $('<div class="formAjouterLien">\
		<form>\
			<div class="titreFormAjouterLien">' + _libelleAjouterLien + '</div>\
			<div class="contenuFormAjouterLien">\
				<div class="pretty-input dark"><label for="titreLien">'+ _libelleTitre + ' :</label><input autocomplete="off" name="titreLien" id="titreLien" type="text" value="" title="' + _libelleTitreTooltip + '" size="20" /></div>\
				<div class="pretty-input dark"><label for="urlLien">' + _libelleUrl + ' :</label><input autocomplete="off" name="urlLien" id="urlLien" type="text" value="" title="' + _libelleUrlTooltip +'" size="30" /></div>\
			</div>\
			<div class="boutons">\
				<a class="button btn" href="#">'+_libelleAnnuler+'</a>\
				<a class="button btn" href="#">'+_libelleAjouter+'</a>\
			</div>\
		</form>\
	</div>');
	// Initialisation des boutons "Ajouter" et "Annuler" du formulaire d'ajout
	// Keypress Handler
	// ---------------------------------------------------------------------------------------------------------------   
	$formAjouterLien.bind("keypress", function (e) {
		if (e.keyCode && e.keyCode == 13) {
			$formAjouterLien.find("a.button:eq(1)").click();
		}
	});
	$formAjouterLien.find(".ajouterLienOV").on("click", function (e) {
		e.preventDefault();
		$(".boiteLiensUtiles .lienActionDetail").menu("close");
	});
	$formAjouterLien.find("a.button").bind("click", function (e) {
		e.preventDefault();
		if ($(this).text() == _libelleAjouter) {
		    // Bouton Ajouter
		    var $inputs = $(".formAjouterLien input");
		    // TODO : Call AJAX pour créer le lien dans Sharepoint
		    // on success : créer le le lien et l'ajouter dans le ul, vider les inputs, fermer le formulaire
		    var error = [];
		    $inputs.each(function () {
		        if ($(this).val().trim() == "") {
		            error.push($(this).attr("id"));
		            $(this).addClass("error");
		        }
		    });
		    if (error.length > 0) {
		        $("p.error").remove();
		        $inputs.filter(":last").parent().after("<p class='error'>" + _libelleTousChampsObligatoires + "</p>");
		        return false;
		    }
		    // Call AJAX pour envoyer les infos du nouveau lien au Backend
		    // TODO : changer l'url

		    $.ajax({
		        url: _urlPageActions,
		        dataType: "html",
		        type: "POST",
		        data: {
		            "action": "ajouterLien",
		            "nom": $inputs.filter("#titreLien").val(),
		            "url": $inputs.filter("#urlLien").val(),
		            "variation":_RootVariation
		        },
		        success: function (data) {
		            // On ajoute le HTML markup du nouveau lien si la requete ajax a fonctionnée
		            // On bind le bouton pour supprimer le lien
		            var lien = $inputs.filter("#urlLien").val();
		            if (!lien.match("://")) {
		                lien = "http://" + lien;
		            }
		            $(".boiteLiensUtiles ul").append($("<li><a target='_blank' href='" + lien + "'>" + $inputs.filter("#titreLien").val() + "</a></li>").append($("<a class='supprimable' idListe='" + data + "' href='#'>x</a>").bind("click", function () {
		                e.preventDefault();
		                if (confirm(_messageSupprimerLien)) {
		                    var $li = $(this).parent();
		                    // Call AJAX pour supprimer le lien de la liste
		                    // TODO : changer l'url
		                    $.ajax({
		                        url: _urlPageActions,
		                        dataType: "json",
		                        type: "POST",
		                        data: {
		                            "action": "supprimerLien",
		                            "id": $(this).attr("idListe")
		                        },
		                        success: function (response) {
		                            $li.remove();
		                        },
		                        error: function (jqXHR, textStatus, errorThrown) {
		                            alert(_texteErreurAJAX);
		                        }
		                    });
		                }
		            })));
		            $inputs.val("");
		            $(".boiteLiensUtiles .lienActionDetail").menu("close");
		        },
		        error: function (jqXHR, textStatus, errorThrown) {
		            alert(_texteErreurAJAX);
		        }
		    });
		}

		else {
		    $(this).parents("form").find("input").val("");
		    $(".boiteLiensUtiles .lienActionDetail").menu("close");
		}
		return false;
	});
	$(".boiteLiensUtiles .lienActionDetail").after($formAjouterLien);
	// Initialisation du bouton "Ajouter un lien" qui toggle le formulaire
	$(".boiteLiensUtiles .lienActionDetail").menu({
		target: $(".formAjouterLien"),
		beforeOpen: function () {
			this.target.find("input").val("");
			this.target.find(".error").removeClass("error").filter("p").remove();
		},
		afterOpen: function () {
			this.target.find("input").eq(0).focus();
		},
		position: { bottom: "5em", left: "1em" }
	});
	// Initialisation des boutons pour supprimer les liens
	$(".boiteLiensUtiles li a.supprimable").bind("click", function (e) {
		e.preventDefault();
		if (confirm(_messageSupprimerLien)) {
			var $li = $(this).parent();
			// Call AJAX pour supprimer le lien de la liste
			// TODO : changer l'url
			$.ajax({
				url: _urlPageActions,
				dataType: "html",
				type: "POST",
				data: {
					action: "supprimerLien",
					id: $(this).attr("idListe")
				},
				success: function (response) {
					$li.remove();
				},
				error: function (jqXHR, textStatus, errorThrown) {
					alert(_texteErreurAJAX);
				}
			});
		}
	});
	var cache;

	// ---- SI on est sur une page avec de l'annotation ------ //
	if (document.body.id == "gabarit-accueil" || document.body.id == "gabarit-comite" || (document.body.id == "gabarit-seanceconseil" )) {
		// Ordre du jour
		var dfdOrdre = new $.Deferred();

		var urlParts = window.location.pathname.split("/");

		// remonte au niveau du site et ajoute path jusqu'aux séances
		if (document.body.id == "gabarit-seanceconseil") {
			urlParts.splice(-2, 2, "OrdreDuJour");
		} else {
			urlParts.splice(-2, 2, "seances", "OrdreDuJour");
		}

		var listUrl = urlParts.join("/");

		var date = $.trim($("#valeurDateContenu").text());
		if (date != "") {
			var myRequest = $.ajax({
				url: _urlPageActions,
				dataType: "json",
				type: "POST",
				data: {
					"action": "obtenirOrdreDuJour",
					"urlListe": listUrl,
					"dateSeance": date
				},
				success: function (data) {
					var $ol = $("<ol />");
					var $liPremierNiveau;
					var $niveauPointCourant = 1;
					var $niveauPointPrecedent = 1;
					var $htmlOutput = "<ol>";


					for (var i = 0; i < data.length; i++)
					{
					    $niveauPointCourant = data[i].Niveau;
                        
						data[i].TitrePoint = $('<div/>').html(data[i].TitrePoint).text();
						data[i].ListeDocument = $('<div/>').html(data[i].ListeDocument).text();
                       
					    // Quand on passe de sous-point à régulier
						if ($niveauPointPrecedent == 2 && $niveauPointCourant == 1) {
						    $htmlOutput = $htmlOutput + "</li></ol></li>";
						}

						// Quand on passe de régulier à sous-point
						if ($niveauPointPrecedent == 1 && $niveauPointCourant == 2) {
						    $htmlOutput = $htmlOutput + "<ol>";
						}
                        
					    // Quand on passe de sous-point à sous-point
						if ($niveauPointPrecedent == 2 && $niveauPointCourant == 2) {
						    $htmlOutput = $htmlOutput + "</li>";
						}

					    // Toujours
						$htmlOutput = $htmlOutput + "<li>" + data[i].TitrePoint;

						
						// Menu de documents						
						if (data[i].ListeDocument != null) {
							var $html = $(data[i].ListeDocument);
							var $links;
							if ($html.find("a").length > 0) {
								$links = $html.find("a");
							} else {
								$links = $html.filter("a");

							}
							if ($links.length > 0) {
							    var $ul = $("<ul />");
							    $links.each(function () {
							        $ul.append($("<li><a href='" + $(this).attr("href") + "'>" + $.trim($(this).text()) + "</a></li>"));
							    });							    
							}
							else
							{
							    var $ul = $("<ul />");
							    $ul.append("<li>" + $html.text() + "</li>");							    
							}

						}
                        
						if ($ul.html() != "<li></li>") {
						    $htmlOutput = $htmlOutput + "<ul>" + $ul.html() + "</ul>";
						}

					    //$ol.append($liPremierNiveau);

						$niveauPointPrecedent = $niveauPointCourant;
					}

					$htmlOutput = $htmlOutput + "</ol>";

				    // Ce bouton ne doit pas etre visible s'il y a deja un ordre du jour présent.					
					if ($("div.ordreDuJour > ol").length > 0) {
					    $("#ExportezSeanceVersWord").hide();
					}

					$(".boiteProchaineSceance .ordreDuJour").append($htmlOutput);
				}
			}).done(function () {
				dfdOrdre.resolve("done");
			});

			myRequest.fail(function (jqXHR, textStatus) {
			    //alert("Request failed: " + textStatus);
			});


		} else {
			dfdOrdre.resolve("noquery");
		}

		$.when(dfdOrdre).done(function () {
			// Drop down prochaines séances
			// -------------------------------------------------------------------------------------------
			var $formSeance = $("<div style='display:none;' id='boiteProchaineSeancePopup'>\
    <div class='inner'>\
    <div class='fichier'>\
        <div class='destination' />\
        <div class='origine' />\
    </div>\
    <ul>\
        <li><a href='#' class='button ico pdf lecture'>" + _libelleOuvrir + "</a></li>\
        <li><a href='#' class='button ico inexistant annoter'>" + _libelleOuvrirAnnoter + "</a></li>\
        <li><a href='#' class='button ico anterieure inactif'>" + _libelleVersionsAnterieures + "</a></li>\
    </ul>\
    </div>\
    </div>");

			// ------ Click d'un bouton du popup ------ //
			$formSeance.find('a.button').bind("click", popupClick);

			// On ajoute le menu popup pour les actions des liens de séances à la suite de la liste de liens
			$(".boiteProchaineSceance a:last").after($formSeance);

			// Événements des différents liens des séances
			// -------------------------------------------------------------------------------
			var dateSeance = $("#valeurDateContenu").text().replace(/\d{2}:\d{2}:\d{2}|[^0-9\-]/g, "");
			var existants = {};

			var getData = function () {
				return $.ajax({
					url: _urlPageActions,
					type: "POST",
					dataType: "json",
					data: {
						action: "obtenirDocumentsAnnotes",
						nomBibliotheque: _bibliothequeDestination,
						currentWebUrl: _CurrentWebUrl,
						"dateSeance": dateSeance
					},
					success: function (data) {
						existants = data;
						verifierDocPDF();
					},
					error: function (jqXHR, textStatus, errorThrown) {
						// alert("Bibliothèque inexistante");
					}
				});
			};

			getData().complete(
				function (jqXHR, textStatus) {
					$(".boiteProchaineSceance a").not(".lienActionDetail").each(function () {
						var self = this;
						$(self).menu({
							target: $formSeance,
							beforeOpen: function () {
								//target.replaceWith(_formSeance.clone(true));
								var dlg = this;
								_urls = { origine: null, destination: null };

								getData().done(function () {
									$(dlg).attr("href", "#").addClass("inactif");
									var $target = dlg.target;
									/* urls
									* document d'origine  : /origine/document.pdf
									* document à vérifier : /destination/aaaa-mm-jj/document.doc
									*/
									// On place l'url du fichier d'origine dans la la div.fichier pour pouvoir l'utiliser dans le popup
									var origine = $(self).attr("href");
									$target.find(".fichier .origine").html(origine);
									_urls.origine = origine;
									var filename = origine.substr(origine.lastIndexOf("/") + 1);
									filename = unescape(filename);

									var destination;
									if (_CurrentWebUrl == _root) {
										destination = _root + _bibliothequeDestination + "/" + dateSeance + "/" + filename;
									}
									else {
										destination = _root + _bibliothequeDestination + _CurrentWebUrl + dateSeance + "/" + filename;
									}

									var destination = existants[filename] ? _root + existants[filename] : destination;
									_urls.destination = destination;
									$target.find(".fichier .destination").html(destination);

									//Retrouver l'extension du document (pdf, doc...)
									var extension = filename.substr(filename.lastIndexOf(".") + 1);
									if (extension.lastIndexOf("?") != -1)
										extension = extension.substr(0, extension.lastIndexOf("?")).toLowerCase();

									//Pour traiter les documents pdf non annotables
									if (origine.indexOf("?ann=0") > -1) {
										$target.find("a.button:eq(1)").closest("li").css("display", "none");
										$target.find("a.button:eq(2)").closest("li").css("display", "none");
									}
									else {
										$target.find("a.button:eq(1)").closest("li").css("display", "block");
										$target.find("a.button:eq(2)").closest("li").css("display", "block");
									}

									// Classes css pour les icones
									//quand le pdf n'existe pas remplacer extention avec celle de du bon documentfilename
									if (!ListDocPDF[filename.substr(0, filename.lastIndexOf("."))]) {
										$target.find("a.button:eq(0)").attr("class", "button ico " + extension + " lecture");
									} else {
										$target.find("a.button:eq(0)").attr("class", "button ico pdf lecture");
									}

									$target.find("a.button:eq(1)").attr("class", "button ico " + extension + " inexistant annoter");

									//Si on est pas avec l'application d'annotation de Mirego, et que le document est un pdf.
									if (navigator.userAgent.indexOf("paperless") == -1 && existants[filename] && extension == "pdf") {
									    $target.find('a.annoter').text(_libelleOuvrirDocumentAnnote);
									}
									else {
									    $target.find('a.annoter').text(_libelleOuvrirAnnoter);
									}

									$target.find('a.lecture')[0].href = origine;

									//Si le document annoté existe.
									if (existants[filename]) {
										//Si on est pas avec l'application d'annotation de Mirego, et que le document est un pdf.                                
										if (navigator.userAgent.indexOf("paperless") == -1 && extension == "pdf") {
											$target.find('a.annoter')[0].href = destination;
										}

										$target.find("a.button").removeClass("inexistant inactif");
									} else {
										//Si le document annoté n'existe pas.
										//Si on est pas avec l'application d'annotation de Mirego, et que le document est un pdf.
										if (navigator.userAgent.indexOf("paperless") == -1 && extension == "pdf") {
											$target.find("a.button:eq(1)").addClass("inactif");
										}
										$target.find("a.button:eq(2)").addClass("inactif");
									}
									if (_usager == _bibliothequeDestination) {
										$target.find('a.button:eq(1)').addClass("inactif")
										$target.find('a.button:eq(2)').addClass("inactif")
									}
								});
							}
							// FIN beforeOpen
						});
						// FIN plugin menu
					});
					// FIN boucle each sur les liens de scéances
				}
			);
		});
	}


	function verifierDocPDF() {

		var ListeHrefDoc = [];
		var listeURL = $(".boiteProchaineSceance li a");

		listeURL.each(function (i) {

			var docHref = $(this).attr("href");
			ListeHrefDoc.push(this.href);

		});

		AjaxVerifierDocPDF(ListeHrefDoc);
	}
	function AjaxVerifierDocPDF(ListeHrefDoc) {

		$.ajax({
			url: _urlPageActions,
			type: "POST",
			dataType: "json",
			data: {
				action: "filtrerListeDocPDF",
				ListeHrefDoc: ListeHrefDoc.toString()
			},
			success: function (data) {
				ListDocPDF = data;
			},
			error: function (xhr, ajaxOptions, thrownError) {
				var x = xhr.status;
			}
		});
	};

    ///// page seance conseil
	if (document.body.id == "gabarit-seanceconseil") {

	    $("#copierTousLesOrderesVersAnglais .copierOrdres").on("click", function () {
	        $.ajax({
	            url: _urlPageActions,
	            type: "POST",
	            dataType: "json",
	            data: {
	                action: "copierTousLesOrderesVersAnglais",
	                dateSeance: $("#valeurDateContenu span").text()
	            },
	            beforeSend: function (xhr, opts) {
	                $("#copierTousLesOrderesVersAnglais .copierOrdres").parent().hide();
	                $("#copierTousLesOrderesVersAnglais .boitePatientez").show();
	                $("#copierTousLesOrderesVersAnglais .boiteMsg").hide();
	            },
	            success: function (obj) {
	                if (obj.ok) {
	                    $("#copierTousLesOrderesVersAnglais .boitePatientez").hide();
	                    $("#copierTousLesOrderesVersAnglais .boiteMsg").show().text(_messageConfirmationCopie);
	                } else {
	                    $("#copierTousLesOrderesVersAnglais .boitePatientez").hide();
	                    $("#copierTousLesOrderesVersAnglais .boiteMsg").show().text(obj.msg);

	                    $("#copierTousLesOrderesVersAnglais .copierOrdres").parent().show();
	                }
	            },
	            error: function () {
	                $("#copierTousLesOrderesVersAnglais .boitePatientez").hide();
	                $("#copierTousLesOrderesVersAnglais .boiteMsg").show().text(_messageErreurCommunication);

	                $("#copierTousLesOrderesVersAnglais .copierOrdres").parent().show();
	            }
	        })
	    });

	    $("#copierOrdresDuJour .copierOrdres").on("click", function () {
	        $.ajax({
	            url: _urlPageActions,
	            type: "POST",
	            dataType: "json",
	            data: {
	                action: "copierOrdresDuJour",
	                dateOrdres: $("#copierOrdresDuJour select option:selected").text(),
	                dateSeance: $("#valeurDateContenu span").text()
	            },
	            beforeSend: function (xhr, opts) {
	                if ($("input[Title=DateSeance]").val() == "") {
	                    $("#copierOrdresDuJour .boiteMsg").show().text(_messageDateSeanceObligatoire);

	                    xhr.abort();
	                } else {
	                    $("#copierOrdresDuJour .copierOrdres").parent().hide();
	                    $("#copierOrdresDuJour .boitePatientez").show();
	                    $("#copierOrdresDuJour .boiteMsg").hide();
	                }
	            },
	            success: function (obj) {
	                if (obj.ok) {
	                    $("#copierOrdresDuJour .boitePatientez").hide();
	                    $("#copierOrdresDuJour .boiteMsg").show().text(_messageConfirmationCopie);
	                } else {
	                    $("#copierOrdresDuJour .boitePatientez").hide();
	                    $("#copierOrdresDuJour .boiteMsg").show().text(obj.msg);

	                    $("#copierOrdresDuJour .copierOrdres").parent().show();
	                }
	            },
	            error: function () {
	                $("#copierOrdresDuJour .boitePatientez").hide();
	                $("#copierOrdresDuJour .boiteMsg").show().text(_messageErreurCommunication);

	                $("#copierOrdresDuJour .copierOrdres").parent().show();
	            }
	        })
	    });

	    $("#ExportezSeanceVersWord .btnExportezSeanceVersWord").on("click", function () {
	        var dateSeance = $("#valeurDateContenu span").text();
	        $.download(_urlPageActions, 'action=ExportezSeanceVersWord&dateSeance=' + dateSeance);
	    });

	    jQuery.download = function (url, data, method) {
	        //url and data options required
	        if (url && data) {
	            //data can be string of parameters or array/object
	            data = typeof data == 'string' ? data : jQuery.param(data);
	            //split params into form inputs
	            var inputs = '';
	            jQuery.each(data.split('&'), function () {
	                var pair = this.split('=');
	                inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
	            });
	            //send request
	            jQuery('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>')
                .appendTo('body').submit().remove();
	        };
	    };

	}



});
// FIN document.ready

function popupClick(e) {
	e.preventDefault();
	
	var button = e.target ? e.target : (e.srcElement ? e.srcElement : null);

	if ($(button).hasClass("inactif")) {
		return false;
	}

	// ----------- URLs -----------
	var origine = _urls.origine;
	var destination = _urls.destination;
	var extension = origine ? (origine.substr(origine.lastIndexOf(".") + 1).toLowerCase()) : "";
	if (extension.lastIndexOf("?") != -1)
		extension = extension.substr(0, extension.lastIndexOf("?"));

	// Bouton "Ouvrir et annoter"
	// -------------------------------------------------------------------------------
	if ($(button).text().match(new RegExp(_regExpBoutonOuvrirAnnoter, "i"))) {
		// Si le fichier existe, on l'ouvre dans une nouvelle page
		if (!$(button).attr("class").match("inexistant")) {
			g_varSkipRefreshOnFocus = true;
			//Ouvrir le pdf annotable sur le IPad
			if (extension == "pdf") {
				window.location.href = destination + "?ann=1";
			}
			else {
				//On présuppose que c'est un document éditable dans Sharepoint
				editDocumentWithProgID2(destination, '', 'SharePoint.OpenDocuments', '0', window.location.host, '0'); //Pour documents office
			}
		}
		//FIN fichier existe
		else {
			// SINON on lance la requete AJAX pour créer le fichier dans la bibliotheque de l'usager
			var $self = $(button);

			//Détection si un document est déjà en cours d'ouverture.
			var isAjaxRunning = $self.hasClass('ajax-running');

			if (isAjaxRunning) {
				return false;
			}
			else {
				$self.addClass('ajax-running');
			}

			// Requete AJAX pour faire créer le fichier pour annotation
			// -------------------------------------------------------- 
			$.ajax({
				url: _urlPageActions,
				type: "POST",
				dataType: "html",
				data: {
					action: "annoter",
					'origine': origine,
					'destination': destination,
					'currentWebUrl': _CurrentWebUrl
				},
				success: function (response) {
					$self.removeClass('ajax-running');
					$self.closest("ul").find(".inexistant, .inactif").removeClass("inexistant inactif");
					g_varSkipRefreshOnFocus = true;

					//Ouvrir le pdf annotable sur le IPad
					if (extension == "pdf") {
						window.location.href = destination + "?ann=1";
					}
					else {
						//On présuppose que c'est un document éditable dans Sharepoint
						editDocumentWithProgID2(destination, '', 'SharePoint.OpenDocuments', '0', window.location.host, '0'); //Pour documents office                           
					}
				},
				error: function (jqXHR, textStatus, errorThrown) {
					$self.removeClass('ajax-running');
					alert(_texteErreurAJAX);
				}
			});
			// FIN requete AJAX
		}
		//Fichier n'existe pas                               
	}
	else if ($(button).text().match(new RegExp(_regExpBoutonOuvrirDocumentAnnote, "i"))) {
		// Bouton Ouvrir le document annoté
		// ------------------------------------------------------------------------------------------
		//S'il y a un document annoté
		if (!$(button).attr("class").match("inactif")) {
			window.location.href = destination;
		}
	} else if ($(button).text().match(new RegExp(_regExpBoutonOuvrir, "i")) && !$(button).text().match(new RegExp(_regExpBoutonOuvrirDocumentAnnote, "i"))) {
		// Bouton Ouvrir
		// ------------------------------------------------------------------------------------------
		//quand le pdf n'existe pas ouvrir en lecture le bon document
		var filename = origine.substr(origine.lastIndexOf("/") + 1);

		if (!ListDocPDF[filename.substr(0, filename.lastIndexOf("."))]) {
			window.location.href = origine;
		} else {
			window.location.href = origine.substr(0, origine.lastIndexOf(".")) + ".pdf";
		}

	} else if ($(button).text().match(/version/i) && !$(button).attr("class").match("inactif")) {
		// Bouton Version antérieures
		// ------------------------------------------------------------------------------------------
		var url = _versionsUrl + "?FileName=" + destination + "&IsDlg=1";
		OpenPopUpPage(url);
	}
}
