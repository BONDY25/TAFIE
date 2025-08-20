# Toolbox for APIs to Fix Integration Errors

![TAFIE LOGO](TAFIE/TAFIE_Logo.JPG)

* ___This is an ongoing project, features and documentation are being added/updated all the time, it is possible some documentation may be out of date and not reflect features since added or updated. - Aiden Bond 01/05/2025___

The current Carrier Integration solution does not clearly define when there are errors, nor does it allow for easy resolution when there are problems. When a label cannot be generated, a generic error message is presented to the Agent with no viewable explanation as to the cause. At regular intervals throughout the day (or interactively against a specific load note) all open errors are listed and a supervisor works through the resolutions. This process is time-consuming, inefficient, and the lack of agency taken from the packers causes unnecessary delays.

Over-Arching Project joining up the three phases

## PHASE 1: 
Introduce an App where a load note can be scanned and a the WCMS error is returned, along with suggestions on how to fix this. These will draw upon our existing table of errors that we have built up over the past few months. At this stage, the App is “read only” where it presents a solution, it will not perform any fixes.

This will achieve Goal 1 by allowing an agent to easily view why an error has occurred and suggesting a fix that can be done at that point.

## PHASE 2: 
Provide interaction with the Warehouse Operative that allows problems to be fixed at that point. If it has been identified as a ‘Data Issue’ then the agent can correct it on the fly. Alternatively, it can be flagged as needing supervisor attention at that point along with notes. Supervisors can run regular reports to see all ‘raised’ issues without needing to wait for timed reports, or for problems to be highlighted directly. Notes can be sorted against an error to allow communication to flow between departments

This will achieve Goal 2 and 3, as well as start on Goal 4 by allowing the Warehouse Operative to fix some problems themselves, as well as allowing agents to instantly highlight to supervisors their problems.

## PHASE 3: 
Problem orders can be marked for ‘Manual Label’ in the same way as ‘Marked for Supervisor’. Admin Teams will be able to select raised orders and using a direct link to the data stored in the Elucid Database, create CSV files for import in the specific format, and en masse.

This will achieve Goal 5 by speeding up Manual Order creation and minimising errors during re-key. Stretch goals will also allow for direct API calls for the label, as well as potentially allowing manual labels to be requested at point of despatch instead.

---

# Useful Links
* [Carrier Module Database Dictionary](CarrierModuleDatabaseDictionary.md)
* [Carrier Module Class Index](ClassIndex.md)
* [Parcel Hub API Docs](https://api.parcelhub.net/docs/)
* [ProCarrier API Docs](https://github.com/Whistl-Fulfilment-South-West/TAFIE/blob/master/ProCarrier_API_Specs_v1.17.pdf)

